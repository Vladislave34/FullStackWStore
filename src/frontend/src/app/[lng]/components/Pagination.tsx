import {number} from "yup";
import {PaginationItem} from "@/app/[lng]/UI/PaginationItem";


const Pagination = ({nums, currentPage}:{nums:number[], currentPage: number}) => {
    return (
        <div className="mt-4 flex gap-4">
            {nums.map((num:number)=> <PaginationItem key={num} num={num} currentPage={currentPage} />)}

        </div>
    );
};

export default Pagination;