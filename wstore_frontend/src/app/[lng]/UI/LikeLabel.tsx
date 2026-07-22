


"use client"
import {FaHeart, FaRegHeart} from "react-icons/fa";
import {favouriteApi} from "@/services/favouriteService";
import type {MouseEvent} from "react";

const LikeLabel = ({ id }: { id: string }) => {
    const { data, isLoading: isLoading_ } = favouriteApi.useIsFavouriteQuery(id);
    const [addFavourite, { isLoading: isAdding }] = favouriteApi.useAddFavouritesMutation();
    const [removeFavourite, { isLoading: isRemoving }] = favouriteApi.useRemoveFavouritesMutation();
    const isLoading = isAdding || isRemoving || isLoading_;

    const handleToggle = async (e: MouseEvent) => {
        e.stopPropagation();
        try {
            if (data) {
                await removeFavourite(id).unwrap();
            } else {
                await addFavourite(id).unwrap();
            }
        } catch (err) {
            console.error("Favourite toggle failed:", err);
        }
    };

    if (isLoading_) return null; // isLoading тут не годиться, бо тоді при кожній мутації елемент зникатиме

    return (
        <div
            className={`absolute top-2 right-2 bg-[var(--accent-soft)] rounded-lg p-1 ${
                isLoading ? "opacity-50 pointer-events-none" : ""
            }`}
            onClick={handleToggle}
        >
            {data ? (
                <FaHeart className="text-xl hover:cursor-pointer" />
            ) : (
                <FaRegHeart className="text-xl hover:cursor-pointer" />
            )}
        </div>
    );
};

export default LikeLabel;