import { Container, createTheme, CssBaseline, ThemeProvider } from "@mui/material";
import { useState } from "react";
import { Outlet } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import Header from "./Header";
import 'react-toastify/dist/ReactToastify.css';
// this is the main component which holds everything
function App() {
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
