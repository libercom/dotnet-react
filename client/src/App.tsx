import { Box, Button, Container } from "@mui/material";
import Navbar from "./components/Navbar";
import Signin from "./components/Signin";
import DataTable from "./components/Table";

const App = () => {
    return (
        <Container>
            <Navbar></Navbar>
            <DataTable></DataTable>
            <Button
                sx={{ mt: 10 }}
                size="large"
                variant="contained"
                color="success"
            >
                Add
            </Button>
            <Signin></Signin>
        </Container>
    );
};

export default App;
