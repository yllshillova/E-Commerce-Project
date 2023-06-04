import { Container, createTheme, CssBaseline, ThemeProvider } from "@mui/material";
import { useCallback, useEffect, useState } from "react";
import { Outlet } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import Header from "./Header";
import 'react-toastify/dist/ReactToastify.css';
import LoadingComponent from "./LoadingComponent";
import { useAppDispatch } from "../store/configureStore";
import { fetchBasketAsync } from "../../features/basket/basketSlice";
import { fetchCurrentUser } from "../../features/account/AccountSlice";
// this is the main component which holds everything
function App() {
  const dispatch = useAppDispatch();
  const [loading, setLoading] = useState(true);
  const initApp = useCallback(async () =>  {
    try {
      await dispatch(fetchCurrentUser());
      await dispatch(fetchBasketAsync());
    } catch (error) {
      console.log(error);
    }
  },[dispatch]);


// e kem perdor Callback function per arsyje qe mos me hi nloop pasi ndispatch kem me bo set user ose set basket
// qe munet bo cause naj loop qe mu thirr sa her t bohet render e kjo e man nmen kshtu qe kur tthirret ne useEffect nuk bohet loop.
  useEffect(() => {
    initApp().then(() => setLoading(false));
  }, [initApp])


  const [darkMode, setDarkMode] = useState(false);
  const paletteType = darkMode ? 'dark' : 'light';
  const theme = createTheme({
    palette: {
      mode: paletteType,
      background: {
        default: paletteType === 'light' ? '#eaeaea' : '#121212'
      }
    }
  })
  function handleThemeChange() {
    setDarkMode(!darkMode);
  }

  if (loading) return <LoadingComponent message="Initialising app..."></LoadingComponent>





  // outlet e vendosim ne container per shkak te navigimit per shkak se ajo zevendsohet me secilin root varesisht se cilin component e prekim na 
  //psh nese ne navigojm te catalog ather outlet eshte ekuivalente me <Catalog /> etj.
  return (
    <ThemeProvider theme={theme}>
      <ToastContainer position="bottom-right" hideProgressBar theme="colored" />
      <CssBaseline />
      <Header darkMode={darkMode} handleThemeChange={handleThemeChange} />
      <Container>
        <Outlet />
      </Container>

    </ThemeProvider>
  );
}


export default App;
