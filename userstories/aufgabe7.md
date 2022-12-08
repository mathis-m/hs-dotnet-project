# User Stories for Exercise 7

## Assign driver to truck
As a user, I want to assign a truck, which is owned by the company, to a hired driver so that the driver can drive the truck to another location.

**ACs:**  
- only employed drivers can be assigned
- only owned trucks can be assigned to
- only one driver can be assigned to a truck, in case it was already assigned it will assign it to the newly requested driver
- if the driver was assigned to another truck, the truck should be then be set unasigned


## Request truck relocate
As a user, I want to request a relocation of an truck, so that I can bring it to the start locations of a tender.

**ACs:**  
- if the truck has no location the request is not permitted (not possible is not allowed by data model, nothing to do here)
- if the truck is not owned the request is not permitted
- if the truck has an on going relocation the request(status != `arrived`) is not permitted
- if a relocation was requested the relocation status should be set to:
    - **case truck location is equal to the requested location:** `arrived`
    - **case no driver is assigned to the truck:** `waiting for driver`
    - **case truck has assigned driver:** `relocation started`
- if a relocation was requested the relocation target location should be set to the requested location


## Assign tender to truck
As a user, I want to assign an tender to a truck, so that I can complete tenders.

**ACs:**  
- if a truck is executing another tender the operation is not permitted
- if the truck is in an ongoing relocation the opteration is not permitted
- if the tender was not accepted the operation is not permitted
- if the truck is not owned the operation is not permitted
- if the tender is not valid(tbd. for now a tender is valid all the time) or is already assigned it is not permitted to assign the truck to the tender.
- if the truck's type, max capacity or current location does not match the tender's requirements(good type, weight or start location) the operation is not permitted
- if the tender is assigned the corresponding truck relocation request is placed
- if the tender is assigned the relation is saved