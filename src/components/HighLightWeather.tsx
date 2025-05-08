import { useCurrentWeather } from "../services/useWeather";
const HighLightWeather = () => {
  const { weather, nameCity, temperature, icon, description } =
    useCurrentWeather();
  return (
    <div className="flex justify-between w-full">
      <div className="w-2/3">
        <div className="flex w-full h-1/2 flex-col ml-10 justify-center items-start">
          <h2 className="lg:text-3xl sm:text-2xl font-bold">
            {nameCity}: {weather}
          </h2>
          <p className="text-s text-gray-400">
            Weather description: {description}
          </p>
        </div>
        <div className="flex w-full h-1/2 flex-col ml-10 justify-center items-start">
          <p className="lg:text-5xl sm:text-3xl font-bold mt-2">
            {temperature}°
          </p>
        </div>
      </div>

      <div className="w-1/3 flex justify-center items-center">
        <img
          src={`http://openweathermap.org/img/wn/${icon}.png`}
          alt={weather}
          className="mx-auto w-70 h-auto"
        />
      </div>
    </div>
  );
};

export default HighLightWeather;
