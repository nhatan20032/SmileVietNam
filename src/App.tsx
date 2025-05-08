import SearchBar from "./components/SearchBar";
import WeatherMainMenu from "./components/WeatherMainMenu";
import WeatherInWeak from "./components/WeatherInWeek";

function App() {  

  return (
    <div className="App-container min-h-screen flex flex-col">
      <SearchBar />
      <div className="flex h-auto">
        <WeatherMainMenu />
        <WeatherInWeak />        
      </div>
    </div>
  );
}

export default App;
