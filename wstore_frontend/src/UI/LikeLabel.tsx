"use client"
import {FaHeart, FaRegHeart} from "react-icons/fa";
import {useState} from "react";


const LikeLabel = () => {
    const [isLike, setIsLike] = useState(false);
    return (
        <div
            className="absolute top-2 right-2"
            onClick={() => setIsLike(prev => !prev)}
        >
            {isLike ?
                <FaHeart className=" text-2xl hover:cursor-pointer" />
                :
                <FaRegHeart className=" text-2xl hover:cursor-pointer" />
            }

        </div>
    );
};

export default LikeLabel;