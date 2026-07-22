'use client'
import {useState} from "react";
import {useDispatch} from "react-redux";
import {useAppDispatch} from "@/hooks/redux";
import {addToOrder, deleteFromOrder} from "@/store/reducers/cartItemSlice";


const CheckBox = ({id} : {id:string}) => {
    const dispatch = useAppDispatch();
    const [isChecked, setIsChecked] = useState(false);

    const handleCheckboxChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const checked = e.target.checked;
        setIsChecked(checked);
        if (checked) {
            dispatch(addToOrder(e.target.value));
        } else {
            dispatch(deleteFromOrder(e.target.value));
        }

    };
    return (
        <div className="flex items-center">
            <input
                id="default-checkbox"
                type="checkbox"
                value={id}
                checked={isChecked}
                onChange={handleCheckboxChange}
                className="w-6 h-6 border border-default-medium rounded-xs bg-neutral-secondary-medium focus:ring-2 focus:ring-brand-soft"
            />
        </div>
    );
};

export default CheckBox;