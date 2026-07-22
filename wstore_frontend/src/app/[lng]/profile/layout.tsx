'use client'
import SideBar from "@/app/[lng]/components/SideBar";
import SideBarLabel from "@/app/[lng]/UI/SideBarLabel";
import {useAppSelector} from "@/hooks/redux";
import EditProfileForm from "@/app/[lng]/UI/forms/EditProfileForm";
import {ReactNode} from "react";
import {usePathname, useRouter} from "next/navigation";
import {useT} from "next-i18next/client";




const Layout = ({children} : {children: ReactNode}) => {
    const user = useAppSelector(state => state.authSlice.user);
    const router = useRouter();
    if(user==null){
        router.push("/");
    }
    const {t} = useT('profile');
    const path = usePathname();
    const currentLng = path.split('/')[1];
    // console.log(user);
    // console.log("dfvsdfv" + process.env.NEXT_PUBLIC_GOOGLE_CLIENT_ID)
    return (
        <>
        <div className="pt-18">
            <div className="flex items-start">
            <SideBar >
                {/*<SideBarLabel href={`/${currentLng}/profile/details`}  >{t("details")}</SideBarLabel>*/}
                {/*<SideBarLabel href={`/${currentLng}/profile/details`}  >{t("details")}</SideBarLabel>*/}
                <SideBarLabel
                    href={`/${currentLng}/profile/details`}
                    subItems={[
                        { href: `/${currentLng}/profile/details/cards`, label: "Cards" },
                        { href: `/${currentLng}/profile/details/adrresses`, label: "Adrress" },
                    ]} >
                    {t("details")}
                </SideBarLabel>
                <SideBarLabel href={`/${currentLng}/profile/cart`} >{t("cart")}</SideBarLabel>
                <SideBarLabel href={`/${currentLng}/profile/my-orders`} >{t("my_orders")}</SideBarLabel>
                <SideBarLabel href={`/${currentLng}/profile/favourite`} >{t("favourite")}</SideBarLabel>
                {user?.roles.includes("StoreOwner") ?
                    <SideBarLabel
                        href={`/${currentLng}/profile/store`}
                        subItems={[
                            { href: `/${currentLng}/profile/store/settings`, label: t('settings') },
                            { href: `/${currentLng}/profile/store/products?page=1`, label: t('products') },
                            { href: `/${currentLng}/profile/store/orders`, label: t('orders') },
                            { href: `/${currentLng}/profile/store/statistics`, label: t('statistics') }
                        ]} >
                        {t("store")}
                    </SideBarLabel>
                        :
                    <SideBarLabel href={`/${currentLng}/profile/store`} >{t("store")}</SideBarLabel>

                }



            </SideBar>
            <div className="w-full">{children}</div>

            </div>
        </div>

        </>
    );
};

export default Layout;