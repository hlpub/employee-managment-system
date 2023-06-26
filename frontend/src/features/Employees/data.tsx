
//Definetely not the best wat to store config values. Used this only because of time limitations.
//Also, if the API is exposed to the Internet, it should be served overt https. Otherwise
//is better to proxy the requests through the FE server. 

import React from "react";
import Employee from "../../shared/employee";

export const baseApiUrl = "http://localhost:5000";


export const populateGrid = (setError: React.Dispatch<React.SetStateAction<string>>,
    setEmployees: React.Dispatch<React.SetStateAction<Employee[] | null>>, querySearch: string, adminKey: string) => {

    fetch(`${baseApiUrl}/employees/${querySearch}`, { headers: { 'x-admin-key': adminKey } })
        .then(resp => {

            if (resp.status === 200)
                return resp.json();

            if (resp.status === 401)
                setError("Admin key is not valid");

            return null;
        })
        .then(res => {

            if (Array.isArray(res)) {

                setEmployees(res.map((x: any) => ({ ...x, dateOfJoining: new Date(x.dateOfJoining) })));
                setError("");
            }

        })
        .catch(() => {
            setError("Employees couldn't be retrieved");
        });
}