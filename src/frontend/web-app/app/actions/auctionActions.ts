'use server'

import { PageResult, Auction, Bid } from "@/types";

import { FieldValues } from "react-hook-form";
import { revalidatePath } from "next/cache";
import { fetchWrapper } from "../lib/fetchWrapper";

export async function getData(
    pageNumber: number, pageSize: number,
    searchTerm: string, orderBy: string, filterBy: string,
    seller?: string | undefined | null, winner?: string | undefined | null): Promise<PageResult<Auction>> {

    const response = await fetchWrapper
        .get(`/search?pageNumber=${pageNumber}&pageSize=${pageSize}&searchTerm=${searchTerm}&orderBy=${orderBy}&filterBy=${filterBy}&seller=${seller}&winner=${winner}`);

    return response;
}

export async function updateAuction(data: FieldValues, id: string) {
    const res = await fetchWrapper.put('/auctions/' + id, data);
    revalidatePath(`/auctions/${id}`);
    return res;
}

export async function createAuction(data: FieldValues) {
    return await fetchWrapper.post('/auctions', data);
}

export async function deleteAuction(id: string) {
    return await fetchWrapper.del('/auctions/' + id);
}
export async function getDetailedViewData(id: string): Promise<Auction> {
    return await fetchWrapper.get('/auctions/' + id);
}

export async function getBidsForAuction(id: string): Promise<Bid[]> {
    return await fetchWrapper.get('/bids?auctionId=' + id);
}

export async function placeBidForAuction(auctionId: string, amount: number) {
    return await fetchWrapper.post(`/bids?auctionid=${auctionId}&amount=${amount}`, {});
}