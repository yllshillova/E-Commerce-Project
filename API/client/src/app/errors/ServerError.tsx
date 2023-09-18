import { Container, Paper, Typography, Divider } from "@mui/material";
import { useLocation } from "react-router-dom";

export default function ServerError() {
    // e kem perdor ket reactHook qe me mujt me access state te agent.ts qe e kem perdor te router.navigate()
    // state e kem perdor qe me mujt me i shfaq informatat e errorit si stacktrace etc...
    const { state } = useLocation();
    return (
        <Container component={Paper}>
            {/* nese ka state naj error shfaqet divi i par nese jo shfaqet veq nje mesazh hard coded Server error */}
            {state?.error ? (
                <>
                    <Typography gutterBottom variant="h3" color='secondary'>
                        {state.error.title}
                    </Typography>
                    <Divider />
                    <Typography variant="body1">{state.error.detail || 'Internal server error'}</Typography>
                </>
            ) : (
                <Typography variant='h5' gutterBottom>Server error</Typography>
            )}
        </Container>
    )
}