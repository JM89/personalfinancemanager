#  MET_02 - Bank Management
 
## List

![MET_02 (1).png](../../static/img/pfm/MET_02%20(1).png)

Actions available: 
* Sort
* Create
* Update
* Delete (with confirmation): disabled if the bank is used by at least one account. Delete the favourite branch object too. 

## Create / Edit bank

![MET_02 (2).png](../../static/img/pfm/MET_02%20(2).png)

| Property      | Type                   | Description                                                                                                                                            | Required | Size | Other constraints                                                                                                                                                     |
| ------------- | ---------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------ | -------- | ---- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Country       | List                   | List of available countries in the system.                                                                                                             | Yes      |      |                                                                                                                                                                       |
| Name          | Text                   | Bank name                                                                                                                                              | Yes      | 50   | Unique.                                                                                                                                                               |
| Existing logo | Text + Preview         | If a logo has been already uploaded (Edit or Failed Validation on Create after uploading), then we display the icon filename and we preview the image. | Yes      |      | Field displayed Under Conditions.                                                                                                                                     |
| Upload logo   | Image Upload + Preview | Upload button + Preview of the uploaded image. If an existing logo exists, a button will be available to upload a new file.                            | Yes      |      | Field displayed Under Conditions. The file should not empty. A file must have been selected. The file should not exceed 200000 bytes.	A file must have a unique name. |
| Website       | Text                   | Website                                                                                                                                                | Yes      | 255  |                                                                                                                                                                       |
| Phone Number  | Text                   | Phone Number (General Enquiries / Customer Services). Without extension number.                                                                        | Yes      | 11   | Pattern 00000000000                                                                                                                                                   |

## Favorite Branch Properties

:warning: Bank Branch is not supported anymore. 