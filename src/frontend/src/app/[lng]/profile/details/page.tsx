
import EditProfileForm from "@/app/[lng]/UI/forms/EditProfileForm";
import {Metadata} from "next";
import {getT} from "next-i18next/server";

type PageProps = { params: Promise<{ lng: string }> }

export async function generateMetadata({ params }: PageProps): Promise<Metadata> {
    const { lng } = await params
    const { t } = await getT('pages', { lng })
    return { title: t('details') }
}

export default function Details() {
    return (
        <div className="p-8 flex justify-center items-center min-h-screen ">
           <EditProfileForm />
        </div>
    );
};

