import TodayWeatherChildComponent from "./TodayWeatherChildComponent";

const TodayWeather = () => {
  return (
    <div className="bg-[#202b3b] h-fit flex w-full flex-col justify-between p-4 rounded-xl">
      <div className="w-full">
        <header className="m-3 text-gray-500 font-bold">
          Today's Forecast
        </header>
        <TodayWeatherChildComponent />
      </div>
    </div>
  );
};

export default TodayWeather;
