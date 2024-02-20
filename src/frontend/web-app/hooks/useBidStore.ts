import { Bid } from "@/types";
import { createWithEqualityFn } from "zustand/traditional"

type State = {
    bids: Bid[];
    open: boolean;
}

type Action = {
    setBids: (bids: Bid[]) => void;
    addBid: (bid: Bid) => void;
    setOpen: (value: boolean) => void;
}

const initialState: State = {
    bids: new Array<Bid>(),
    open: true
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
    },

    setOpen: (value: boolean) => {
        set(() => ({
            open: value
        }))
    }
}));