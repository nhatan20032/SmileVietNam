const SearchBar = () => {
  return (
    <header className="App-header mt-3 w-2/3 p-4">
      <input
        type="text"
        placeholder="Search for cities"
        className="App-search h-[3.5rem] w-full p-2 text-white rounded-xl bg-[#2c3e50]"
      />
    </header>
  );
};

export default SearchBar;
