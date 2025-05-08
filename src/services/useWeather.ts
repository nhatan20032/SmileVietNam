import { useEffect, useState } from "react";
import Axios from "axios";

export interface WeatherForecast {
  temp: number;
  icon: string;
  dt_txt: string;
}

export const useCurrentWeather = () => {
  const [weather, setWeather] = useState("");
  const [nameCity, setNameCity] = useState("");
  const [temperature, setTemperature] = useState(0);
  const [icon, setIcon] = useState("");
  const [description, setDescription] = useState("");
  const [feelLike, setFeelLike] = useState<number>();
  const [windSpeed, setWindSpeed] = useState<number>();
  const [humidity, setHumidity] = useState<number>();
  const [cloud, setCloud] = useState<number>();

  useEffect(() => {
    const fetchWeather = async () => {
      try {
        const res = await Axios.get(
          "https://api.openweathermap.org/data/2.5/weather?lang=vi&appid=9c430f59eb9f8d987f5296640f5f80f1&id=1581130"
        );

        setWeather(res.data.weather[0].main);
        setNameCity(res.data.name);
        setTemperature(Math.round(res.data.main.temp - 273.15));
        setIcon(res.data.weather[0].icon);
        setDescription(res.data.weather[0].description);
        setFeelLike(Math.round(res.data.main.feels_like - 273.15));
        setWindSpeed(res.data.wind.speed);
        setHumidity(res.data.main.humidity);
        setCloud(res.data.clouds.all);
      } catch (err) {
        console.error("Error fetching weather data:", err);
      }
    };

    fetchWeather();
  }, []);

  return {
    weather,
    nameCity,
    temperature,
    icon,
    description,
    feelLike,
    windSpeed,
    humidity,
    cloud,
  };
};

export const useTodayForecast = () => {
  const [forecasts, setForecasts] = useState<WeatherForecast[]>([]);

  useEffect(() => {
    Axios.get(
      "https://api.openweathermap.org/data/2.5/forecast?lang=vi&appid=9c430f59eb9f8d987f5296640f5f80f1&id=1581130"
    )
      .then((res) => {
        const today = new Date().toISOString().split("T")[0];

        const filtered = res.data.list
          .filter((item: any) => item.dt_txt.startsWith(today))
          .slice(0, 5)
          .map((item: any) => ({
            temp: Math.round(item.main.temp - 273.15),
            icon: item.weather[0]?.icon ?? "",
            dt_txt: new Date(item.dt_txt).toLocaleTimeString([], {
              hour: "2-digit",
              minute: "2-digit",
            }),
          }));

        setForecasts(filtered);
      })
      .catch((err) => console.error("Lỗi gọi API", err));
  }, []);

  return { forecasts };
};
