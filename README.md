
# <h1>Hall of Fame Creator Stats Website </h1>
## <h2>Description</h2>
<p>This website intends to provide a way to navigate or slideshow your Cities: Skylines II images captured by the excelent mod created by Toverux outside of the game itself, as well as provide stats about them.</p>

### <h3>Important Note</h3>
<p>Please use this app with moderation by abiding to the following rules of conduct:
  <ul>
    <li>Do not leave the slideshow running if you are not paying attention or afk;</li>
    <li>Do not in any way attempt to use this code abusively in such way that might lead to any type of harm or damage to anyone or thing.</li>
  </ul>
</p>

## <h2>Features</h2>
<p>Below is a list of features provided by the website:
  <ul>
    <li>List all your images from all your cities uploaded using the Hall of Fame mod for Cities: Skylines 2;</li>
    <li>Provide your stats on the mod (nº of screenshots, total of views and likes);</li>
    <li>Show info about each of your images:
      <ul>
        <li>City name;</li>
        <li>Total likes;</li>
        <li>Total views;</li>
        <li>Likes and views variation since you last refeshed or visited the page<sup>1</sup>;</li>
        <li>Date of the image submission to Hall of Fame</li>
      </ul>
    </li>
    <li>See the images fullscreen and slideshow them;</li>
    <li>Reorder the list without reloading all data according to the following criteria:
      <ul>
        <li>Most recently submitted (default);</li>
        <li>Most liked;</li>
        <li>Most likes per day;</li>  
        <li>Best relation between likes and views;</li>
        <li>Most views per day;</li>
        <li>Most total views;</li>
        <li>Largest likes variation since your last visit<sup>1</sup>;</li>
        <li>Largest views variation since your last visit<sup>1</sup>;</li>
      </ul>
    </li>
    <li>Show an interactive line graph/chart with the image's views and likes progression over time (can suppress each line individually and can zoom in and pan on chart for enhanced detail);</li>
  </ul>

  <sub><sup>1</sup> The website stores datapoints on a local database for all images on each page load or refresh, and also for the particular image when the requested to see its stats chart. This datapoint collection allows for the stats to be shown on charts and help calculating the likes and views variation since your last visit. For more datails refer to the section <strong>How it works</strong>.<sub>
</p>

## <h2>How to use it</h2>
<p>Clicking an image thumbnail on the list will bring up a larger version of it along with the usual image navigation and slideshow functionalites.</p>
<p>Pressing the data below each thumbnail will bring up a chart showing the views and likes progression over time of the image.<sup>2</sup></p>
<p>Dragging over an area of the chart will zoom in the selected portion of the data. You can then use the pan button on the top right hand of the chart to switch the zooming functionality to panning, allowing you to navigate the chart when zoomed.</p>
<p>You can reset the zoom at any time by pressing the button next to the panning/zoom switch button.</p>
<p>Views and Likes lines can individually be hidden/shown by pressing the matching labels on the bottom left hand of the chart.<p>
<sub><sup>2</sup> <small>Each time a chart is opened, it also stores and individual datapoint, so if you leave the page opened for some time, the views and likes count presented on the list may differ from the ones being presented as current on the chart, since its using freshly updated data as opposed to the data used on the list that was gathered when you loaded/refreshed the page.</sub>

## <h2>How it works</h2>
### <h3>Basic principles</h3>
<p>
  The website connects to the Hall of Fame mod API and requests your data, which includes all your submitted images and their stats and then stores these stats on a local database.
</p>

### <h3>Some more advanced details</h3>
<p>
  Not only each image's data is stored if yet nonexistant (newly created images), or gets its stats updated on the image table on the database, but it also creates a datapoint on a distinct table, where it stores the likes and views count alongside a timestamp of the insertion.<br/>
  Storing these datapoints is crucial, since without them we would be left with just the current views and likes count of each image and as such it would be impossible to show its progression over time that is expressed on the charts.

  Having data captured is essential for making the graphs meaningful, otherwise they will be mostly flatlines with the occasional jump, but at the same time, its not like views and likes are happening to your images every second, minute, hour or even day, so there is no need to be refreshing the page everytime.<br/> 
  In fact, <strong><underline>its counterproductive to keep refreshing the page</underline></strong>, since each time you refresh it, not only datapoints are added, which in shorts bursts are meaningless since 99.9% or the time they will just repeat the same data as the last datapoint stored due to the likely lack of variation, but it negates one of my favorite features of the website, the likes and views variation since your last visit.<br/>
  Since i did not wanted to recurr to storing a cookie with your last visit date, which could easely bypass what I'm about to describe, the variations are obtained by comparing the views and likes data from the main table fetched before updating with the newly received data from the API which occurs upon page load (let me remind your that, as explained on the features, the reorderings happen solely on the UI, and do not refresh/reload the page).
  
  So, now you're asking yourself: <i>'when should I refresh the page to get new data?'</i> Well, thats really up to you... but i would say that every 4 hours is more than enough to over time have a good chunk of data to compose the charts.<br/>
  To help automate the data gathering, the homepage accepts a parameter <strong><i>scheduled</i></strong> via querystring. You can create a scheduled task on your OS to call your website silently every X hours by adding the parameter with the value <strong>true</strong>. This allows the website to bypass updating the stats of the existing images on the main table, only creating datapoints for them on the datapoints table, while still adding any image you might have submitted in the meantime.<br/>
  This enables datapoints gathering while still maintaining the data used for the variations intact untill you next visit the site.
</p>

## <h2>Technical details</h2>
### <h3>Base details</h3>
<p>This solution was built using .Net 8.0 Core Razor pages.</p>
<p>It uses two LocalDbs, one to store the images data, and the other as a sinc for the logs. These databases are generated automatically when you first run the project but you will need to run the migrations present on their respective folders inside the <i>Data</i> and the <i>Logging</i> folders.</p>
<p>When deployed to your local IIS or webserver, you should create 2 MSSQL databases with the same structure of the localDbs so that the website then uses a persistant and more robust engine. The connection strings on the <i>appSettings.json</i> file need to be updated to your own reality when deploying.</p>

### <h3>How to set up the website settings</h3>
<p><strong>Mandatory: </strong>You need to edit the appsettings.config file by replacing the following placeholders:
  <ul>
    <li>{CreatorName}: Your Paradox username (the one you use in your game);</li>
    <li>{CreatorId}: You can find your CreatorId on your mod folder tipically at <em>'%appdata%\LocalLow\Colossal Order\Cities Skylines II\ModSettings\HallOfFame\HallOfFame.coc'</em>;</li>
    <li>{HardwareId}: Any simple string identifier should suffice as long as you stick to it.</li>
    <li>{DbCreatorId}: This needs to be requested to Morgan Toveroux, the actual Hall of Fame mod creator on the <em>Cities: Skylines Modding</em> Discord server</li>
  </ul>
</p>

## <h2>Disclaimer</h2>
<p>This website does not intend in any way to replace or make obsolete the original mod, and as such, the image views on this app do not add up to the view count saved by the mod.</p>
<p>It out of the scope of this file to provide instructions on how to import, build or deploy your website and required databases or scheduled automation.</p>
<p>
  Toverux's mod code can be found at:
  <ul>
    <li>https://github.com/toverux/HallOfFame</li>
  </ul>
  
  And you can find the mod itself at:
  <ul>
    <li>https://mods.paradoxplaza.com/mods/90641/Windows</li>
  </ul>  
</p>

## <h2>Special thanks to</h2>
<ul>
  <li>Morgan Toverux, above all, for developing the amazing mod that inspired me to develop this app, and for all his support on the development without which this would all had been impossible.</li>
  <li>The entire CS2 community on discord that has provided me with support along the times, in particular Dragonae who helped me get through 'The Plague' by getting out of his way and downloading my 300k pop. city and finding the faulty links among such massive grid;</li>
  <li>Sully and all the community on his server for the local assets and ideias provided;</li>
  <li>All the modders and asset creators, without whom this game would be dead already and that keep amazing me with the extensions they provide to it everyday;</li>
  <li>Many Youtube content creators that have provided me with insights, entertainment, inspiration and kept me company for so many hours.</li>
</ul>

## <h2>Finally</h2>
<p>I hope you find this website helpful, and that you keep up with my ocasional spam of contributions, in particular from Calabash, and thank you for all the appreciation shown towards my creations.</p>
<p>I'm Obelix, and I can occasionally be found on the CS2 Modding discord channel.</p>
