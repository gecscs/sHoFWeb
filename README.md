# sHoFSimpleJSONReader

ABOUT
This started by being a way to read from a json file Ids of images (from a creator, me) from the Hall of Fame mod and show them in a webpage able to slideshow them and show their stats.

Since then, it got extensively upgraded with the release of a new API endpoint in HoF that returns all images from a creator and their data. At that point, it evolved into storing the images data in a database and recurrently access the api to update the data. 

By storing the views/likes in a distinct table, we can obtain the progress of both, since each time the website access the data from the API, it creates new entries on this table. This enables the graphs that are presented with the images showing the variation of both over time. 

By comparing the data stored in the main table with the new data from the API, it gets the variation of both and allows it to be presented to the user. This also enables some UI functionalities that alow the user to reorder the screenshots on the page by these and other criteria. 

Reordering the images will not force a reload of the page, and therefore will not refresh the data. This allows the variations functionalities to be meaningfull, otherwise the variation will probably always be 0, unless someone just viewed and/or liked the shot inbetween your initial load and the reorder request.

Viewing the stats graphs, however, will show updated data that can differ from the presented one, for the same reasons just exposed earlier regarding the variations.

The page also allows a paramenter on the url that prevents the refreshed data from being saved to the main images table (except if its a new image that was not present on the DB), only updating the stats table. This allows a scheduled task to be created on your system to call the website recurrently according to the recurrence you set (I use 4h inverval), so that the stats get updated on the stats table to provide more datapoints to the graphs, without messing with the variation data that is calculated using the main table data. This means that if you dont use the website for lets say 3 days, when you finnaly open it, it will show the variation of likes and views over the 3 day timespan, but when you see the graphs, they will show you a more detailed evolution, since there will be a 4h to 4h data to show.
