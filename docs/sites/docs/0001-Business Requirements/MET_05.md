# MET_05 - Expense Type Management

## List
 
![MET_05 (1).png](../../static/img/pfm/MET_05%20(1).png)

Actions available: 
* Sort
* Create
* Update
* Delete (with confirmation): disabled if the expense type is used by at least one expense. 

## Create / Edit expense type

![MET_05 (2).png](../../static/img/pfm/MET_05%20(2).png)
 
| Property          | Type       | Description                                                                            | Required | Size | Other constraints |
| ----------------- | ---------- | -------------------------------------------------------------------------------------- | -------- | ---- | ----------------- |
| Name              | Text       | Expense name                                                                       | Yes      | 50   | Unique.           |
| Graph Color       | Color Pick | Color which will be displayed in the graph in the dashboard for this expense type. | Yes      |      |
| Show On Dashboard | Yes/No     | Determine if the expense types will be in the dashboard                            | Yes      |      |
