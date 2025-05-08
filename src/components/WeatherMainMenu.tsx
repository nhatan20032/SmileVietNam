import AirConditions from "./AirConditions";
import HighLightWeather from "./HighLightWeather";
import TodayWeather from "./TodayWeather";

const WeatherMainMenu = () => {
  return (
    <main className="flex-1 text-white space-y-5 w-2/3 p-4 h-full">
      <HighLightWeather />
      <TodayWeather />
      <AirConditions />
    </main>
  );
};

export default WeatherMainMenu;
