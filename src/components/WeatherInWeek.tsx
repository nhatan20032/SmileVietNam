import SevenDayWeatherRow from "./SevenDayWeatherRow";

const SevenDayForecast = () => {
  return (
    <aside className="App-sidebar w-1/3 text-white border-r rounded-4xl p-4 h-auto flex flex-col m-3">
      <header className="px-5 py-4 text-left text-gray-500 font-bold text-xl">
        7-DAY FORECAST
      </header>
      <SevenDayWeatherRow />
    </aside>
  );
};

export default SevenDayForecast;
