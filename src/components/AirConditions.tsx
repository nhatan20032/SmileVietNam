import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faTemperatureHigh,
  faWind,
  faDroplet,
  faCloud,
} from "@fortawesome/free-solid-svg-icons";
import { useCurrentWeather } from "../services/useWeather";

const AirConditions = () => {
  const { feelLike, windSpeed, humidity, cloud } = useCurrentWeather();

  return (
    <div className="bg-[#202b3b] flex h-fit w-full flex-col justify-between p-4 rounded-xl">
      <div className="w-full flex justify-between items-center px-4">
        <header className="text-gray-500 font-bold text-lg">
          Air conditions
        </header>
        <button className="bg-blue-600 text-white px-4 py-2 rounded-lg">
          See more
        </button>
      </div>
      <div className="w-full">
        <div className="grid grid-cols-2 m-5">
          <div className="font-bold">
            <div className=" text-gray-500 text-lg">
              <FontAwesomeIcon icon={faTemperatureHigh} /> Real Feel
            </div>
            <div className="ml-7 mt-3 text-3xl text-[#C3CAD5]">{feelLike}°</div>
          </div>
          <div className="font-bold">
            <div className=" text-gray-500 text-lg">
              <FontAwesomeIcon icon={faWind} /> Wind
            </div>
            <div className="ml-7 mt-3 text-3xl text-[#C3CAD5]">
              {windSpeed} km/h
            </div>
          </div>
        </div>
        <div className="grid grid-cols-2 m-5">
          <div className="font-bold">
            <div className=" text-gray-500 text-lg">
              <FontAwesomeIcon icon={faDroplet} /> Humidity
            </div>
            <div className="ml-7 mt-3 text-3xl text-[#C3CAD5]">{humidity}%</div>
          </div>
          <div className="font-bold">
            <div className=" text-gray-500 text-lg">
              <FontAwesomeIcon icon={faCloud} /> Cloud
            </div>
            <div className="ml-7 mt-3 text-3xl text-[#C3CAD5]">{cloud}</div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AirConditions;
