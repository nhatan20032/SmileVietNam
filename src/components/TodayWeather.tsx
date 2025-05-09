import {useTodayForecast } from "../services/useWeather";
const TodayWeather = () => {
    const { forecasts } = useTodayForecast();
  return (
    <div className="bg-[#202b3b] h-fit flex w-full flex-col justify-between p-4 rounded-xl">
      <div className="w-full">
        <header className="m-3 text-gray-500 font-bold">
          Today's Forecast
        </header>

        <div className="grid grid-cols-6 divide-x-1 divide-gray-700">
          {forecasts.map((item) => (
            <div className="text-center justify-center">
              <div className="text-gray-500 font-bold">{item.dt_txt}</div>
              <img
                src={`https://openweathermap.org/img/wn/${item.icon}@2x.png`}
                alt="weather icon"
                className="mx-auto w-40 h-auto"
              />
              <div className="text-2xl font-bold">{item.temp}°</div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default TodayWeather;
