'use client'
import * as Yup from "yup";

import UniversalForm from "@/app/[lng]/UI/forms/abstract/UniversalForm";
import FormikInput from "@/app/[lng]/UI/forms/inputs/FormikInput";

import {useT} from "next-i18next/client";

import {addressApi} from "@/services/addressService";
import IAddAddressModel from "@/models/address/IAddAddressModel";
import {useMemo} from "react";



const initialValues: IAddAddressModel = {
    city: "",
    country: "",
    street: "",
    houseNumber: "",
};

const AddAddressForm = ({closeModal}: {closeModal: () => void}) => {
    const [addAddress] = addressApi.useAddAdrressMutation();
    const {t} = useT('address_form');
    const addAddressSchema = useMemo(() => Yup.object({
        city: Yup.string()
            .min(2, t('errors.minLength'))
            .required(t('errors.cityRequired')),
        country: Yup.string()
            .min(2, t('errors.minLength'))
            .required(t('errors.countryRequired')),
        street: Yup.string()
            .min(2, t('errors.minLength'))
            .required(t('errors.streetRequired')),
        houseNumber: Yup.string()
            .required(t('errors.houseNumberRequired')),
    }), [t]);

    const handleSubmit = async (values: IAddAddressModel) => {
        try {
            await addAddress(values).unwrap();

            closeModal();
        } catch (err) {
            console.error(err);
        }
    };

    return (
        <UniversalForm
            initialValues={initialValues}
            validationSchema={addAddressSchema}
            onSubmit={handleSubmit}
            title={t('title')}
            subtitle={t('subtitle')}
            submitLabel={t('submit')}
        >
            <FormikInput<IAddAddressModel>
                name="country"
                label={t('country')}
                type="text"
                placeholder="Ukraine"
            />
            <FormikInput<IAddAddressModel>
                name="city"
                label={t('city')}
                type="text"
                placeholder="Lutsk"
            />
            <FormikInput<IAddAddressModel>
                name="street"
                label={t('street')}
                type="text"
                placeholder="Soborності"
            />
            <FormikInput<IAddAddressModel>
                name="houseNumber"
                label={t('houseNumber')}
                type="text"
                placeholder="12A"
            />
        </UniversalForm>
    );
};

export default AddAddressForm;