import { Container, createTheme, CssBaseline, ThemeProvider } from "@mui/material";
import { useEffect, useState } from "react";
import { Outlet } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import Header from "./Header";
import 'react-toastify/dist/ReactToastify.css';
import { useStoreContext } from "../context/StoreContext";
import { getCookie } from "../util/util";
import agent from "../api/agent";
import LoadingComponent from "./LoadingComponent";
// this is the main component which holds everything
function App() {
  const {setBasket} = useStoreContext();
  const [loading, setLoading]= useState(true);


  useEffect(() =>{
    const buyerId = getCookie('buyerId');
    if(buyerId){
      agent.Basket.get()
        .then(basket=> setBasket(basket))
        .catch(error => console.log(error))
        .finally(() => setLoading(false));
    }
    // testim ekstra nese nuk ka buyerId dikush athere nuk e nalum loading i del veq mesazhi qe empty o basket.
    else{
      setLoading(false);
    }
  },[setBasket])


  const [darkMode, setDarkMode] = useState(false);
  const paletteType = darkMode ? 'dark' : 'light';
  const theme = createTheme({
    palette: {
      mode: paletteType,
      background : {
        default : paletteType === 'light' ? '#eaeaea' : '#121212'
      }
    }
  })
  function handleThemeChange(){
    setDarkMode(!darkMode);
  }

  if(loading) return <LoadingComponent message="Initialising app..."></LoadingComponent>





  // outlet e vendosim ne container per shkak te navigimit per shkak se ajo zevendsohet me secilin root varesisht se cilin component e prekim na 
  //psh nese ne navigojm te catalog ather outlet eshte ekuivalente me <Catalog /> etj.
  return (
    <ThemeProvider theme ={theme}>
      <ToastContainer position="bottom-right" hideProgressBar theme="colored" />
        <CssBaseline />
        <Header darkMode = {darkMode} handleThemeChange = {handleThemeChange} />
        <Container>
        <Outlet />
        </Container>
      
    </ThemeProvider>
  );
}


export default App;
