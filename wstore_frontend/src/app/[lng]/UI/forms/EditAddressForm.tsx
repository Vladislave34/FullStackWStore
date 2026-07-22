'use client'
import * as Yup from "yup";

import UniversalForm from "@/app/[lng]/UI/forms/abstract/UniversalForm";
import FormikInput from "@/app/[lng]/UI/forms/inputs/FormikInput";

import {useT} from "next-i18next/client";

import {addressApi} from "@/services/addressService";
import IUpdateAddressModel from "@/models/address/IUpdateAddressModel";
import IAddressModel from "@/models/address/IAddressModel";

const editAddressSchema = Yup.object({
    city: Yup.string()
        .min(2, "Мінімум 2 символи")
        .required("Місто обов'язкове"),
    country: Yup.string()
        .min(2, "Мінімум 2 символи")
        .required("Країна обов'язкова"),
    street: Yup.string()
        .min(2, "Мінімум 2 символи")
        .required("Вулиця обов'язкова"),
    houseNumber: Yup.string()
        .required("Номер будинку обов'язковий"),
});

interface EditAddressFormProps {
    address: IAddressModel;
    closeModal: () => void;
}

const EditAddressForm = ({address, closeModal}: EditAddressFormProps) => {
    const [editAddress] = addressApi.useEditAddressMutation();
    const {t} = useT('address_form');

    const initialValues: IUpdateAddressModel = {
        id: address.id,
        city: address.city,
        country: address.country,
        street: address.street,
        houseNumber: address.houseNumber,
    };

    const handleSubmit = async (values: IUpdateAddressModel) => {
        try {
            await editAddress(values).unwrap();

            closeModal();
        } catch (err) {
            console.error(err);
        }
    };

    return (
        <UniversalForm
            initialValues={initialValues}
            validationSchema={editAddressSchema}
            onSubmit={handleSubmit}
            title={t('edit_title')}
            subtitle={t('edit_subtitle')}
            submitLabel={t('save')}
        >
            <FormikInput<IUpdateAddressModel>
                name="country"
                label={t('country')}
                type="text"
                placeholder="Ukraine"
            />
            <FormikInput<IUpdateAddressModel>
                name="city"
                label={t('city')}
                type="text"
                placeholder="Lutsk"
            />
            <FormikInput<IUpdateAddressModel>
                name="street"
                label={t('street')}
                type="text"
                placeholder="Soborності"
            />
            <FormikInput<IUpdateAddressModel>
                name="houseNumber"
                label={t('houseNumber')}
                type="text"
                placeholder="12A"
            />
        </UniversalForm>
    );
};

export default EditAddressForm;