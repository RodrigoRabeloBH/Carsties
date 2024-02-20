import { Auction, PageResult } from "@/types";
import { createWithEqualityFn } from "zustand/traditional";

type State = {
    auctions: Auction[];
    totalCount: number;
    pageCount: number;
}

type Action = {
    setData: (data: PageResult<Auction>) => void;
    setCurrentPrice: (auctionId: string, amount: number) => void;
}

const initialState: State = {
    auctions: [],
    pageCount: 0,
    totalCount: 0
}

export const useAuctionStore = createWithEqualityFn<State & Action>((set) => ({
    ...initialState,
    setData: (data: PageResult<Auction>) => {
        set(() => ({
            auctions: data.results,
            totalCount: data.totalCount,
            pageCount: data.pageCount
        }));
    },
    setCurrentPrice: (auctionId: string, amount: number) => {
        set((state) => ({
            auctions: state.auctions.map(a => a.id === auctionId
                ? { ...a, currentHighBid: amount } : a)
        }));
    }
}))
