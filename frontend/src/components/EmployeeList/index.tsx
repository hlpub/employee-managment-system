import { Link, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@mui/material";
import Employee from "../../shared/employee";

const EmployeeList: React.FC<{
    setOpen: React.Dispatch<React.SetStateAction<boolean>>,
    setCurrentEmployee: React.Dispatch<React.SetStateAction<(Employee | null)>>,
    employees: Employee[] | null
}> = ({ setOpen, setCurrentEmployee, employees }) => {

    const openDetail = (employee: Employee) => {

        setOpen(true);
        setCurrentEmployee(employee);
    }


    return employees ? <TableContainer component={Paper} >
        <Table sx={{ minWidth: 950 }} aria-label="simple table">
            <TableHead>
                <TableRow>
                    <TableCell align="right">ID</TableCell>
                    <TableCell>First Name</TableCell>
                    <TableCell>Last Name</TableCell>
                    <TableCell>Email</TableCell>
                    <TableCell>Job Title</TableCell>
                    <TableCell align="right">Date of joining</TableCell>
                    <TableCell align="right">Years of service</TableCell>
                </TableRow>
            </TableHead>
            <TableBody>
                {employees.map(emp => (
                    <TableRow key={emp.id}>
                        <TableCell align="right"><Link sx={{ cursor: "pointer" }} onClick={() => openDetail(emp)}>{emp.id}</Link></TableCell>
                        <TableCell>{emp.firstName}</TableCell>
                        <TableCell>{emp.lastName}</TableCell>
                        <TableCell>{emp.email}</TableCell>
                        <TableCell>{emp.jobTitle}</TableCell>
                        <TableCell align="right">{emp.dateOfJoining?.toDateString()}</TableCell>
                        <TableCell align="right">{emp.yearsOfService}</TableCell>
                    </TableRow>
                ))}
            </TableBody>
        </Table>
    </TableContainer> : <>There are no employees loaded</>;
}

export default EmployeeList