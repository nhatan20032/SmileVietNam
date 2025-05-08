import { useEffect, useState } from "react";
import Axios from "axios";

export interface WeatherOfTheWeek {
  temp_min: number;
  temp_max: number;
  icon: string;
  weather: string;
  day: string;
}

export const useWeatherOfTheWeek = () => {
  const [weatherOfTheWeek, setWeatherOfTheWeek] = useState<WeatherOfTheWeek[]>(
    []
  );

  useEffect(() => {
    Axios.get(
      "https://api.openweathermap.org/data/2.5/forecast?lang=vi&appid=9c430f59eb9f8d987f5296640f5f80f1&id=1581130"
    )
      .then((res) => {
        const grouped: { [date: string]: any[] } = {};

        // Nhóm theo ngày
        res.data.list.forEach((item: any) => {
          const date = item.dt_txt.split(" ")[0];
          if (!grouped[date]) grouped[date] = [];
          grouped[date].push(item);
        });

        const today = new Date().toISOString().split("T")[0];

        const getShortWeekday = (dateStr: string) => {
          const d = new Date(dateStr);
          const days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
          return days[d.getDay()];
        };

        const weekly: WeatherOfTheWeek[] = Object.entries(grouped)
          .filter(([date]) => date >= today)
          .map(([date, items]) => {
            const { temp_min, temp_max } = items[0].main;
            const icon = items[0].weather[0]?.icon ?? "";
            const weather = items[0].weather[0]?.main ?? "";
            const day = getShortWeekday(date);
            return { temp_min, temp_max, weather, icon, day };
          })
          .slice(0, 7);

        setWeatherOfTheWeek(weekly);
      })
      .catch((err) => console.error("Lỗi gọi API", err));
  }, []);

  return { weatherOfTheWeek };
};
