'use server'

import { PageResult, Auction } from "@/types";
import { getTokenWorkaround } from "./authActions";

export async function getData(pageNumber: number, pageSize: number, searchTerm: string, orderBy: string, filterBy: string): Promise<PageResult<Auction>> {

    const res = await fetch(`http://localhost:6001/search?pageNumber=${pageNumber}&pageSize=${pageSize}&searchTerm=${searchTerm}&orderBy=${orderBy}&filterBy=${filterBy}`);

    if (!res.ok)
        throw Error('Failed to fetch data');

    return res.json();
}

export async function updateAuctionTest() {
    const data = {
        mileage: Math.floor(Math.random() * 10000) + 1
    }

    const token = await getTokenWorkaround();

    const res = await fetch('http://localhost:6001/auctions/6a5011a1-fe1f-47df-9a32-b5346b289391', {
        method: 'PUT',
        headers: {
            'Content-type': 'application/json',
            'Authorization': 'Bearer ' + token?.access_token
        },
        body: JSON.stringify(data)
    });

    if (!res.ok) return { status: res.status, message: res.statusText }

    return res.statusText;
}