import { Bid } from "@/types";
import { createWithEqualityFn } from "zustand/traditional"

type State = {
    bids: Bid[],
}

type Action = {
    setBids: (bids: Bid[]) => void;
    addBid: (bid: Bid) => void;
}

const initialState: State = {
    bids: [] as Array<Bid>
}

export const useBidStore = createWithEqualityFn<State & Action>((set) => ({
    ...initialState,
    setBids: (bids: Bid[]) => {
        set(() => ({
            bids: bids
        }))
    },
    addBid: (bid: Bid) => {
        set((state) => ({
            bids: !state.bids.find(x => x.id === bid.id) ? [bid, ...state.bids] : [...state.bids]
        }))
    }
}));