import { useWeatherOfTheWeek } from "../services/dateWeather";

const SevenDayWeatherRow = () => {
  const { weatherOfTheWeek } = useWeatherOfTheWeek();

  return (
    <div className="flex-1 grid grid-rows-6 divide-y divide-gray-700 gap-x-4">
      {weatherOfTheWeek.map((item, index) => (
        <div
          key={index}
          className="flex items-center justify-between text-center px-5 py-3"
        >
          <div className="w-1/3 text-gray-500 font-bold">{item.day}</div>
          <div className="w-1/3 flex flex-row items-center justify-center ">
            <img
              src={`https://openweathermap.org/img/wn/${item.icon}@2x.png`}
              alt="weather icon"
              className="w-auto h-auto"
            />
            <span>{item.weather}</span>
          </div>
          <div className="w-1/3">
            <span className="font-bold">{Math.round(item.temp_max - 273.15)}°</span>/
            <span className="text-gray-500 font-bold">{Math.round(item.temp_min - 273.15)}°</span>
          </div>
        </div>
      ))}
    </div>
  );
};

export default SevenDayWeatherRow;
