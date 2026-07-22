"use client"
import React, { useState } from 'react';
import {
    BarChart,
    Bar,
    XAxis,
    YAxis,
    CartesianGrid,
    ResponsiveContainer,
    Cell,
} from 'recharts';
import { statisticsApi } from "@/services/statisticsService";
import CustomSelect from "@/app/[lng]/UI/SelectButton";

interface CategoryIncomeItem {
    startDate: string;
    endDate: string;
    category: string;
    categoryUk: string;
    income: number;
}

interface CustomBarLabelProps {
    x?: number;
    y?: number;
    width?: number;
    value?: number;
}

const CustomBarLabel = (props: CustomBarLabelProps) => {
    const { x, y, width, value } = props;
    if (x === undefined || y === undefined || width === undefined) return null;
    return (
        <text
            x={x + width / 2}
            y={y - 8}
            textAnchor="middle"
            fontSize={13}
            fontWeight={600}
            fill="var(--text)"
        >
            {value}
        </text>
    );
};

export default function CategoryIncomeChart({ lng }: { lng: string }) {


    const { data: rawData, isLoading, isError } = statisticsApi.useGetStatisticsByCategoryQuery();

    if (isLoading) {
        return (
            <div className="w-full rounded-xl p-5 border-2" style={{ height: 460, background: 'var(--surface)', borderColor: 'var(--border)', color: 'var(--text)' }}>
                Завантаження...
            </div>
        );
    }

    if (isError || !rawData) {
        return (
            <div className="w-full rounded-xl p-5 border-2" style={{ height: 460, background: 'var(--surface)', borderColor: 'var(--border)', color: 'var(--sale)' }}>
                Не вдалося завантажити дані
            </div>
        );
    }

    const chartData = (rawData as CategoryIncomeItem[]).map((item) => ({
        name: lng === "en" ? item.category : item.categoryUk,
        income: item.income,
    }));

    return (
        <div
            className="w-full rounded-xl p-5 border-2"
            style={{ height: 460, background: 'var(--surface)', borderColor: 'var(--border)' }}
        >
            <div className="flex flex-row w-full justify-between">
                <div className="mb-2 text-lg font-semibold" style={{ color: 'var(--text)' }}>
                    Прибуток за категорією
                </div>
            </div>
            <ResponsiveContainer width="100%" height="85%">
                <BarChart
                    data={chartData}
                    margin={{ top: 30, right: 30, left: 10, bottom: 10 }}
                >
                    <CartesianGrid stroke="var(--border)" vertical={false} />
                    <XAxis
                        dataKey="name"
                        stroke="var(--muted)"
                        tick={{ fontSize: 13, fill: 'var(--muted)' }}
                    />
                    <YAxis
                        stroke="var(--muted)"
                        tick={{ fontSize: 13, fill: 'var(--muted)' }}
                        domain={[0, 'dataMax + 500']}
                    />
                    <Bar
                        dataKey="income"
                        fill="var(--price)"
                        radius={[6, 6, 0, 0]}
                        barSize={60}
                        label={<CustomBarLabel />}
                        isAnimationActive={false}
                    >
                        {chartData.map((_, i) => (
                            <Cell key={i} fill="var(--price)" />
                        ))}
                    </Bar>
                </BarChart>
            </ResponsiveContainer>
        </div>
    );
}