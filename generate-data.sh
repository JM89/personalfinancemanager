client_secret="RTkyA3RNh4cHHhS8ftXe17WOQu9a0Jjd"
api_endpoint="https://localhost:4431" #"https://localhost:7098" #

auth_response=$(curl -L POST "http://localhost:8080/realms/pfm/protocol/openid-connect/token" -H 'Content-Type: application/x-www-form-urlencoded' --data-urlencode 'client_id=pfm' --data-urlencode 'grant_type=password' --data-urlencode "client_secret=$client_secret" --data-urlencode 'scope=openid' --data-urlencode 'username=jess' --data-urlencode 'password=SecurityMatters!123' )

access_token=$( jq -r  '.access_token' <<< "${auth_response}" ) 

if [ -z "$access_token" ]
then
      echo "Authentication failed"
      exit -1
fi

echo "Authentication succeeded"

create_common_expense () {
  expenseType=$1
  dateMovement=$2
  paymentMethod=$3
  cost=$4
  curl -v --insecure -X 'POST' \
      "$api_endpoint/api/Expense/Create" \
        -H 'accept: text/plain' \
        -H "Authorization: Bearer $access_token" \
        -H 'Content-Type: application/json' \
        -d "{
          \"accountId\": 1,
          \"dateExpense\": \"$dateMovement\",
          \"cost\": $cost,
          \"expenseTypeId\": $expenseType,
          \"paymentMethodId\": $paymentMethod,
          \"description\": \"Common Expense for expense type $expenseType\",
          \"hasBeenAlreadyDebited\": true,
          \"paymentMethodHasBeenAlreadyDebitedOption\": true
        }"
  sleep 2
}

create_internal_transfer_expense () {
  expenseType=$1
  dateMovement=$2
  cost=$3
  curl -v --insecure -X 'POST' \
      "$api_endpoint/api/Expense/Create" \
        -H 'accept: text/plain' \
        -H "Authorization: Bearer $access_token" \
        -H 'Content-Type: application/json' \
        -d "{
          \"accountId\": 1,
          \"dateExpense\": \"$dateMovement\",
          \"cost\": $cost,
          \"expenseTypeId\": $expenseType,
          \"paymentMethodId\": 5,
          \"description\": \"Internal Transfer account 1 to 2 for expense type $expenseType\",
          \"hasBeenAlreadyDebited\": true,
          \"paymentMethodHasBeenAlreadyDebitedOption\": true,
          \"targetInternalAccountId\": 2
        }"
  sleep 2
}

create_cash_expense () {
  expenseType=$1
  dateMovement=$2
  atmWithdrawId=$3
  cost=$4
  curl -v --insecure -X 'POST' \
    "$api_endpoint/api/Expense/Create" \
      -H 'accept: text/plain' \
      -H "Authorization: Bearer $access_token" \
      -H 'Content-Type: application/json' \
      -d "{
        \"accountId\": 1,
        \"dateExpense\": \"$dateMovement\",
        \"cost\": $cost,
        \"expenseTypeId\": $expenseType,
        \"paymentMethodId\": 2,
        \"description\": \"Cash Expense for expense type $expenseType\",
        \"hasBeenAlreadyDebited\": true,
        \"paymentMethodHasBeenAlreadyDebitedOption\": true,
        \"atmWithdrawId\": $atmWithdrawId
      }"
  sleep 2
}

x=1
while [ $x -le 5 ]
do

  curl -v --insecure -X 'POST' \
    "$api_endpoint/api/Income/Create" \
    -H 'accept: text/plain' \
    -H "Authorization: Bearer $access_token" \
    -H 'Content-Type: application/json' \
    -d "{
        \"accountId\": 1,
        \"cost\": 1000,
        \"description\": \"Salary 2023-0$x-01\",
        \"dateIncome\": \"2023-0$x-01\"
    }"

  sleep 2

  curl -v --insecure -X 'POST' \
    "$api_endpoint/api/Saving/Create" \
      -H 'accept: text/plain' \
      -H "Authorization: Bearer $access_token" \
      -H 'Content-Type: application/json' \
      -d "{
        \"accountId\": 1,
        \"dateSaving\": \"2023-0$x-15\",
        \"amount\": 500,
        \"targetInternalAccountId\": 3,
        \"description\": \"Saving\"
      }"
  
  sleep 2

  cost=$(( $x*100 ))

  create_common_expense 1 "2023-0$x-03" 1 $cost
  create_common_expense 2 "2023-0$x-16" 1 $cost
  create_common_expense 2 "2023-0$x-19" 3 $cost
  create_common_expense 3 "2023-0$x-21" 4 $cost
  create_internal_transfer_expense 3 "2023-0$x-02" $cost
  create_cash_expense 1 "2023-0$x-11" 1 $x
  create_cash_expense 2 "2023-0$x-19" 1 $x
  create_cash_expense 2 "2023-0$x-21" 2 $x

  x=$(( $x + 1 ))
done

