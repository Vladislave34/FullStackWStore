import {FC, type ReactNode} from "react";

interface NavBarProps {
    children: ReactNode;
}

const SideBar  : FC<NavBarProps> = ({children}) => {
    return (
        <div
            className="flex flex-col  items-center bg-[var(--surface)]
             gap-8 w-[15%] h-screen  border-r-2 border-[var(--border)]
             pt-8
             "
        >
            {children}
        </div>
    );
};

export default SideBar;