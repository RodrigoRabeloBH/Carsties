import { getTokenWorkaround } from "@/app/actions/authActions";
import { FieldValues } from "react-hook-form";

const baseUrl = process.env.API_URL;

async function get(endpoint: string) {
    const requestOptions = {
        method: 'GET',
        headers: await getHeaders()
    };
    const response = await fetch(baseUrl + endpoint, requestOptions);
    return await handleResponse(response);
}

async function post(endpoint: string, body: FieldValues) {
    const requestOptions = {
        method: 'POST',
        headers: await getHeaders(),
        body: JSON.stringify(body)
    };
    const response = await fetch(baseUrl + endpoint, requestOptions);
    return await handleResponse(response);
}

async function put(endpoint: string, body: {}) {
    const requestOptions = {
        method: 'PUT',
        headers: await getHeaders(),
        body: JSON.stringify(body)
    };
    const response = await fetch(baseUrl + endpoint, requestOptions);
    return await handleResponse(response);
}

async function del(endpoint: string) {
    const requestOptions = {
        method: 'DELETE',
        headers: await getHeaders()
    };
    const response = await fetch(baseUrl + endpoint, requestOptions);
    return await handleResponse(response);
}

async function getHeaders() {
    const token = await getTokenWorkaround();
    const headers = { 'Content-type': 'application/json' } as any;
    if (token)
        headers.Authorization = 'Bearer ' + token.access_token;
    return headers;
}

async function handleResponse(response: Response) {
    const text = await response.text();;
    let data;
    try {
        data = JSON.parse(text);
    } catch (error) {
        data = text;
    }

    if (response.ok)
        return data || response.statusText;
    else {
        const error = {
            status: response.status,
            message: typeof data === 'string' ? data : response.statusText
        };

        return { error };
    }
}


export const fetchWrapper = {
    get,
    post,
    put,
    del
}