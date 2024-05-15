# Space Project

## Description
This C# application chooses the best location and day for launch given weather data and criteria for launch.

## Features
- Read weather data csv file for each city from a folder.
- Perform analysis on weather data for each city and chooses best location and day for launch.
- Display results, including input weather data for each city, most suitable day for launch for each city, most suiatble location.

## Installation Instructions
1. Clone this repository to your local machine.
2. Open the project in Visual Studio or any other C# development environment.
3. Build the project to compile the application.

## Usage
1. Launch the application by running the "VVPS_project.sln" file using Visual Studio 2022.
2. Specify the file path to a folder, containing all *WeatherData.csv files, * where is the name of a valid city location for launch. (Kourou, Tanegashima, CapeCanaveral, Kodiak, Mahia).
   ### Use the full input_example folder path, located in the solution to see an example output.
4. Choose if you want to use the default criteria for most suitable day selection. If 'n', you will be prompted to give crieria for each field.
5. View the analysis results displayed on the screen.
6. Choose if you want to send the result to e-mail via outlook.

## Input Data Format
  The weather data csv files must be in a valid foramt:
  #### Each row starts with a header for row field type (day, temperature, wind, humidity, precipitation, lighting, clouds)
  #### The number of rows must be 7 and each row must have a unique header (for each field^)
  #### The nummber of columns cannot be greater than 16 (including header row)
  #### The values in 'day' must be unique.
  #### For each field valid values must be set (Check WeatehrData set methods).
  The name of the csv file must start with a name of a valid location (Kourou, Tanegashima, CapeCanaveral, Kodiak, Mahia)
  and end with WeatherData.csv to be read.

## Examples
Use the full input_example folder path, located in the solution to see an example output.
Example CSV format for CapeCanaveralWeatherData.csv:
##### "Day",1,2,3 
##### "temperature",26,23,33
##### "Wind",11,9,6
##### "Humidity",42,22,70
##### "Precipitation",2,2,4
##### "Lighting","No","No","No"
##### "Clouds","Cumulus","Altocumulus","Altocumulus"
