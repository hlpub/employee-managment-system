import { Box, Button, Modal, Stack, TextField, Typography } from "@mui/material";
import React, { useEffect, useState } from "react";
import { baseApiUrl } from "../../features/Employees/data";
import Employee from "../../shared/employee";

const style = {
    position: 'absolute' as 'absolute',
    top: '40%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: 500,
    bgcolor: 'background.paper',
    border: '2px solid #000',
    boxShadow: 24,
    p: 4,
};


const EmployeeDetail: React.FC<{
    open: boolean, setOpen: React.Dispatch<React.SetStateAction<boolean>>, employee: Employee | null,
    setEmployees: React.Dispatch<React.SetStateAction<Employee[] | null>>
}> = ({ open, setOpen, employee, setEmployees }) => {

    const [currentData, setCurrentData] = useState<Employee | null>(null);
    const [update, setUpdate] = useState(false);
    const [deleteEmp, setDelete] = useState(false);

    useEffect(() => {

        setCurrentData(employee);

    }, [employee]);

    useEffect(() => {

        if (deleteEmp && employee) {

            setDelete(false);

            const apiUri = `${baseApiUrl}/employees/employee/${employee.id}`;
            fetch(apiUri, {
                method: 'DELETE'
            })
                .then(resp => {
                    if (resp.status === 200) {
                        setOpen(false);
                        setEmployees(employees =>
                            employees?.filter(x => x.id !== employee?.id) ?? null);
                    }
                    else {
                        alert('An error has occured. Please try again later');
                    }
                })
                .catch(() => alert('An error has occured. Please try again later'))
        }

    }, [deleteEmp, employee, employee?.id, setEmployees, setOpen]);

    useEffect(() => {

        if (update && currentData) {

            setUpdate(false);

            const apiUri = `${baseApiUrl}/employees/employee` + (employee ? ('/' + employee.id) : '');
            fetch(apiUri, {
                method: employee ? 'PUT' : 'POST',
                body: JSON.stringify(currentData),
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(resp => {

                    if (resp.status === 200) {
                        if (employee)
                            setEmployees(employees => {
                                const index = employees?.findIndex(x => x.id === employee.id) as number;
                                return Object.assign([], employees, { [index]: currentData })
                            });
                        else
                            resp.json().then(res => {

                                const newEmployee: Employee = {
                                    ...res,
                                    dateOfJoining: new Date(currentData.dateOfJoining)
                                };

                                setEmployees(employees => employees?.concat([newEmployee]) ?? [newEmployee]);
                            });

                        setOpen(false);
                    }

                    if (resp.status === 400)
                        resp.text().then(msg => alert(msg));
                })
                .catch(() => alert('An error was thrown by the server.  Try again later'));
        }

    }, [update, currentData, employee, setEmployees, setOpen]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {

        const prop = e.target.dataset.member as string;
        const val = prop === "DateOfJoining" ? new Date(Date.parse(e.target.value)) : e.target.value;

        const newData = { ...currentData, [prop]: val } as Employee;
        setCurrentData(newData);
    }


    const handleClose = () => setOpen(false);
    const handleUpsert = () => setUpdate(true);
    const handleDelete = () => setDelete(true);


    return <Modal
        open={open}
        onClose={handleClose}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
    >
        <Box sx={style}>
            <Typography variant="h6" component="h2">
                {employee ? "Edit employee" : "Add employee"}
            </Typography>

            <TextField sx={{ m: 1 }} inputProps={{ "data-member": "firstName" }} label="First Name" onChange={handleChange} defaultValue={employee?.firstName} variant="standard" /><br />
            <TextField sx={{ m: 1 }} inputProps={{ "data-member": "lastName" }} label="Last Name" onChange={handleChange} defaultValue={employee?.lastName} variant="standard" /><br />
            <TextField sx={{ m: 1 }} inputProps={{ "data-member": "email" }} label="Email" onChange={handleChange} defaultValue={employee?.email} variant="standard" /><br />
            <TextField sx={{ m: 1 }} inputProps={{ "data-member": "jobTitle" }} label="Job Title" onChange={handleChange} defaultValue={employee?.jobTitle} variant="standard" /><br />
            <TextField sx={{ m: 1 }} inputProps={{ "data-member": "dateOfJoining" }} helperText="Date of Joining" onChange={handleChange} type="date" defaultValue={employee?.dateOfJoining?.toISOString().substring(0, 10)} variant="standard" /><br />

            <Stack spacing={1} justifyContent="end" direction="row">
                <Button variant="contained" onClick={handleUpsert}>{employee ? "Update" : "Add"}</Button>
                {employee ? <Button onClick={handleDelete} variant="contained">Delete</Button> : null}
                <Button variant="outlined" onClick={handleClose}>Cancel</Button>
            </Stack>
        </Box>
    </Modal>;
}

export default EmployeeDetail