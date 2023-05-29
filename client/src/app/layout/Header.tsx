import { ShoppingCart } from "@mui/icons-material";
import { AppBar, List, Switch, Toolbar, Typography, ListItem, IconButton, Badge, Box } from "@mui/material";
import { Link, NavLink } from "react-router-dom";
import { useAppSelector } from "../store/configureStore";

const midLinks = [
    { title: 'catalog', path: '/catalog' },
    { title: 'about', path: '/about' },
    { title: 'contact', path: '/contact' },
]
const rightLinks = [
    { title: 'login', path: '/login' },
    { title: 'register', path: '/register' }
]

const navStyles = {
    color: 'inherit',
    textDecoration: 'none',
    typography: 'h6',
    '&:hover': {
        color: 'grey.500'
    },
    '&.active': {
        color: 'text.secondary'
    }
}

interface Props {
    darkMode: boolean;
    handleThemeChange: () => void;
}
export default function Header({ darkMode, handleThemeChange }: Props) {
    // appselector hook e perdorum kur na vyn gjendja e basketit psh ne header per count me na dal ose ne basket page
    const {basket} = useAppSelector(state => state.basket);
    // funksion i cili merr nje vlere te sum 0 dhe mbledh items ne array te caktuar duke kthyer nje shume ne itemCount qe eshte totali i items ne shporte.
    const itemCount = basket?.items.reduce((sum, item) => sum + item.quantity, 0);
    return (
        <AppBar position="static" sx={{ mb: 4 }}>
            <Toolbar sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                {/* title */}
                <Box display='flex' alignItems='center'>
                    <Typography variant="h6" component={NavLink}
                        to='/'
                        sx={navStyles}
                    >
                        E-STORE
                    </Typography>
                    <Switch checked={darkMode} onChange={handleThemeChange} />
                </Box>

                {/* mid links */}
                {/* lista e cila loops neper midlinks pra catalog,about contact */}
                <List sx={{ display: 'flex' }}>
                    {midLinks.map(({ title, path }) => (
                        <ListItem
                            component={NavLink}
                            to={path}
                            key={path}
                            sx={navStyles}
                        >
                            {title.toUpperCase()}
                        </ListItem>
                    ))}
                </List>

                {/* right links */}
                <Box display='flex' alignItems='center'>
                    <IconButton component = {Link} to='/basket' size='large' edge='start' color='inherit' sx={{ mr: 2 }}>
                        <Badge badgeContent={itemCount} color="secondary">
                            <ShoppingCart />
                        </Badge>
                    </IconButton>
                    {/* lista e cila loops neper right links siq jane login, register etj */}
                    <List sx={{ display: 'flex' }}>
                        {rightLinks.map(({ title, path }) => (
                            <ListItem
                                component={NavLink}
                                to={path}
                                key={path}
                                sx={navStyles}
                            >
                                {title.toUpperCase()}
                            </ListItem>
                        ))}
                    </List>
                </Box>


            </Toolbar>
        </AppBar>
    )
}