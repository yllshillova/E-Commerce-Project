import { Box, Typography } from "@mui/material";
import Slider from "react-slick";

export default function HomePage() {
    const settings = {
        dots: true,
        infinite: true,
        speed: 500,
        slidesToShow: 1,
        slidesToScroll: 1
    };
    return (
        <>
            <Slider {...settings}>
                <div>
                    <img src="/images/products/Lenovo-IdeaPad-Gaming-3-15ACH6.png" alt="acer" style={{ display: 'block', width: '100%', maxHeight: 500 }} />
                </div>
                <div>
                    <img src="/images/products/Apple-Watch-S8-GPS-41mm.jpg" alt="watch" style={{ display: 'block', width: '100%', maxHeight: 500 }} />
                </div>
                <div>
                    <img src="/images/products/ASUS-ROG-Strix-G10DK.png" alt="iphone" style={{ display: 'block', width: '100%', maxHeight: 500 }} />
                </div>
                <div>
                    <img src="/images/products/Xiaomi-Imilab-W12-(037617).jpg" alt="hero" style={{ display: 'block', width: '100%', maxHeight: 500 }} />
                </div>
            </Slider>

            <Box display='flex' justifyContent='center' sx={{p: 4}}>
                <Typography variant="h1">
                    Welcome to the shop!
                </Typography>
            </Box>


        </>
    )
}