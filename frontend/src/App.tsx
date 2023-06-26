import { AppBar, Box, CssBaseline, Toolbar, Typography } from '@mui/material';
import Employees from './features/Employees'

const App = () => {
    return <Box sx={{ display: 'flex' }}>
        <CssBaseline />
        <AppBar component="nav">
            <Toolbar>
                <Typography
                    variant="h6"
                    component="div"
                    sx={{ flexGrow: 1, display: { xs: 'none', sm: 'block' } }}>
                    Employee Managment System
                </Typography>
            </Toolbar>
        </AppBar>
        <Box component="main" sx={{ p: 3, mt: 7 }}>
            <Employees />
        </Box>
    </Box>

};

export default App;

