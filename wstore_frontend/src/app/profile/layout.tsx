'use client'
import SideBar from "@/components/SideBar";
import SideBarLabel from "@/UI/SideBarLabel";
import {useAppSelector} from "@/hooks/redux";
import EditProfileForm from "@/UI/forms/EditProfileForm";
import {ReactNode} from "react";
import {useRouter} from "next/navigation";




const Layout = ({children} : {children: ReactNode}) => {
    const user = useAppSelector(state => state.authSlice.user);
    const router = useRouter();
    if(user==null){
        router.push("/");
    }
    // console.log(user);
    // console.log("dfvsdfv" + process.env.NEXT_PUBLIC_GOOGLE_CLIENT_ID)
    return (
        <>
        <div className="pt-18 flex">
            <SideBar >
                <SideBarLabel href="/profile/details"  >Details</SideBarLabel>
                <SideBarLabel href="/profile/my-orders" >My Orders</SideBarLabel>
                <SideBarLabel href="/profile/favourite" >Favourite</SideBarLabel>
                {user?.roles.includes("StoreOwner") ?
                    <SideBarLabel
                        href="/profile/store"
                        subItems={[
                            { href: "/profile/store/settings", label: "Налаштування" },
                            { href: "/profile/store/products", label: "Продукти" },
                        ]} >
                        Store
                    </SideBarLabel>
                        :
                    <SideBarLabel href={"/profile/store"} >Store</SideBarLabel>

                }



            </SideBar>
            {children}
        </div>

        </>
    );
};

export default Layout;