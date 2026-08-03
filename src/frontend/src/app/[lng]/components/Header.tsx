

import Icon from "@/app/[lng]/UI/Icon";
import Image from "next/image";
import SearchBar from "@/app/[lng]/UI/SearchBar";


import SwitchThemeIcon from "@/app/[lng]/UI/SwitchThemeIcon";

import SwitchLocaleIcon from "@/app/[lng]/UI/SwitchLocaleIcon";
// import BurgerMenu from "@/UI/BurgerMenu";
import MobileMenu from "@/app/[lng]/components/MobileMenu";
import GuestMenu from "@/app/[lng]/UI/GuestMenu";
import {TranslationRecord} from "@/types/translations";
import {useAppSelector} from "@/hooks/redux";
import AuthorizeMenu from "@/app/[lng]/components/AuthorizeMenu";
import Menu from "@/app/[lng]/UI/Menu";
import {FaCartShopping} from "react-icons/fa6";
import {FC} from "react";
import {getT} from "next-i18next/server";

import Link from "next/link";
import {headers} from "next/headers";
import NavLink from "@/app/[lng]/UI/NavLink";



interface Props {
    params: Promise<{
        lng :string

    }>;

}


const Header  :FC<Props> = async ({params}) => {
    const {lng} = await params



    const { t } = await getT('nav', { lng });



    return (

        <nav
            style={{
                background: "var(--surface)",
                borderBottom: "1px solid var(--border)",
            }}
            className="w-full  p-4 flex justify-between items-center h-[75px] fixed top-0 z-50">
            {/*Logo */}
            <Link
                href={`/${lng}?page=1`}
                style={{ color: "var(--text)" }}
                className="text-xl font-bold hover:cursor-pointer"
            >
                W
                <span style={{ color: "var(--muted)" }}>Store</span>
            </Link>


            <div
                style={{color: "var(--muted)"}}
                className="hidden md:flex items-center gap-4 text-base lg:gap-6">
                <NavLink lng={lng} choice={"genderId"} gender={"women"}>
                    {t("women")}
                </NavLink>
                <NavLink lng={lng} choice={"genderId"} gender={"men"}>
                    {t("men")}
                </NavLink>
                <NavLink lng={lng} choice={"hasSale"}>
                    {t("sale")}
                </NavLink>







                {/*<span className="hover:text-[var(--text)]*/}
                {/* hover:cursor-pointer transition-colors hover:border-b-2">*/}
                {/*    {t('men')}*/}
                {/*</span>*/}
                {/*<span className="hover:text-[var(--text)]*/}
                {/* hover:cursor-pointer transition-colors hover:border-b-2">*/}
                {/*    {t('sale')}*/}
                {/*</span>*/}
            </div>

            <div className="hidden md:flex flex-row items-center gap-4 justify-end">

                <SearchBar />
                <SwitchLocaleIcon  />
                <SwitchThemeIcon />
                <Icon height={40} width={40}  >
                    {/*<Image src="/shopping-bags.png" alt="Cart" width={24} height={24} priority />*/}
                    <Link href={`/${lng}/profile/cart`}>
                        <FaCartShopping size={22} color="var(--accent-mid)"   />
                    </Link>


                </Icon>
                <Menu  />


            </div>
            {/*<BurgerMenu  />*/}
            <div className="flex md:hidden items-center gap-3">
                <SearchBar />
                <MobileMenu
                    links={[
                        { key: "women", label: t("women") },
                        { key: "men",   label: t("men") },
                        { key: "sale",  label: t("sale") },
                    ]}
                />
            </div>

        </nav>
    );
};

export default Header;