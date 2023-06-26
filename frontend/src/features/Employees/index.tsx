import { Button, Grid, TextField } from "@mui/material";
import { useEffect, useState } from "react";
import EmployeeDetail from "../../components/EmployeeDetail";
import EmployeeList from "../../components/EmployeeList";
import Employee from "../../shared/employee";
import { populateGrid } from "./data";

const Employees = () => {

    const [search, setSearch] = useState("");
    const [querySearch, setQuerySearch] = useState("");
    const [adminKey, setAdminKey] = useState("");
    const [openDetail, setOpenDetail] = useState(false);
    const [currentEmployee, setCurrentEmployee] = useState<Employee | null>(null);
    const [employees, setEmployees] = useState<Employee[] | null>(null);
    const [error, setError] = useState("");

    //Probably if this was a real world app, I would use React query instead

    useEffect(() => {

        populateGrid(setError, setEmployees, querySearch, adminKey);

    }, [querySearch, adminKey]);

    const handleAddEmployee = () => {
        setOpenDetail(true);
        setCurrentEmployee(null);
    }

    const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setSearch(e.target.value);
    }

    const handleAdminChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setAdminKey(e.target.value);
        if (!e.target.value)
            setSearch("");
    }

    const handleSearch = () => {
        setQuerySearch(search);
    }

    return <>
        <Grid container marginBottom={5} minWidth={700}>
            <Grid item xs={4} alignSelf="center">
                <Button variant="contained" onClick={handleAddEmployee}>Add Employee</Button>
            </Grid>
            <Grid item xs={5} display="flex" gap={2} alignItems="center">
                <TextField disabled={adminKey === ""} onChange={handleSearchChange} value={search}
                    label="Search" helperText="Admin key required for searching" variant="standard" />
                <Button sx={{ height: '25px' }} disabled={adminKey === ""} variant="contained"
                    onClick={handleSearch}>Go</Button>
            </Grid>
            <Grid item xs={2}>
                <TextField onChange={handleAdminChange} value={adminKey} label="Admin key" type="password"
                    variant="standard" />
            </Grid>
        </Grid>
        {error ? error :
            <EmployeeList employees={employees} setOpen={setOpenDetail} setCurrentEmployee={setCurrentEmployee} />}
        <EmployeeDetail setEmployees={setEmployees} employee={currentEmployee} open={openDetail}
            setOpen={setOpenDetail} />
    </>;
}

export default Employees