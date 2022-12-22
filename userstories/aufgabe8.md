# User Stories for Exercise 8

## Avg speed
As a user, I want the simulation to take the truck and driver into account, so that truck speed is calculated accordingly.

**ACs:**  
- default avg. of 70km/h
    - Drivers who are dreamy are 2km/h below the avg
    - Drivers who are racers are 3hm/h over the avg
- to reach the avg km/h 7.5 kW per ton are needed
    - if the truck has less power it will be slower than the avg in the same ratio (eg. 10t 60kW, avg driver => 75kW needed has 80% => 80% * 70 km/h = 56 km/h)


## Fuel cost
As a user, I want the simulation to take the driver and the default consumtion of the truck into account, so that the fuel cost is calculated accordingly.

**ACs:**  
- the used fuel will be calculated by the consumption of the truck and for racers with an additional 2,5 percent of this.
- the actual consumtion will be accountet when the truck arrives at the target location, one liter = 1€

## Relocation
As a user, I want the relocation of trucks to be realistic.

**ACs:**  
- drivers can only drive max 8h p day / 7 days a week
- relocations will be finished when the time has went by that was estimated accordingly to the driver and truck.
    - upon relocation of a truck has finished the new location will be assigned to the truck
    - the money for the fuel will be accounted accordingly
    - the status of the relocation will be set to "Arrived"
- relocations can not be canceled