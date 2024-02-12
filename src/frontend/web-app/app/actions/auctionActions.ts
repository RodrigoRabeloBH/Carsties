'use server'

import { PageResult, Auction } from "@/types";

export async function getData(pageNumber: number, pageSize: number, searchTerm: string, orderBy: string, filterBy:string): Promise<PageResult<Auction>> {

    const res = await fetch(`http://localhost:6001/search?pageNumber=${pageNumber}&pageSize=${pageSize}&searchTerm=${searchTerm}&orderBy=${orderBy}&filterBy=${filterBy}`);

    if (!res.ok)
        throw Error('Failed to fetch data');

    return res.json();
}