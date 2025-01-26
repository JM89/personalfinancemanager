# MET_01 - Account Management

## List
 
![MET_01 (1).png](../../static/img/pfm/MET_01%20(1).png)

Actions available: 
* Sort
* Create
* Update
* Delete (with confirmation): disabled if the account is used by at least one expense, income or ATM withdraw. 
* Set as Favorite

## Create / Edit account

![MET_01 (2).png](../../static/img/pfm/MET_01%20(2).png)

| Property        | Type     | Description                                 | Required | Size    | Other constraints              |
| --------------- | -------- | ------------------------------------------- | -------- | ------- | ------------------------------ |
| Bank            | List     | List of available banks in the system.      | Yes      |         |                                |
| Currency        | List     | List of available currencies in the system. | Yes      |         |                                |
| Name            | Text     | Account name                                | Yes      | 50      | Unique.                        |
| Initial Balance | Decimal  | Initial balance                             | Yes      | (10, 2) | Can't be changed in edit mode. |
| Saving Account  | Checkbox | Define if the account is a saving account.  | Yes      |         |                                |

The Current Balance is not visible from the creation screen, it is initialized with Initial Balance at creation time. It is going to be updated when expenses, incomes and ATM withdraw are created. 
