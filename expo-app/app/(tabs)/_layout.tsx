import {Tabs} from 'expo-router';
import React from 'react';

import {TabBarIcon} from '@/components/navigation/TabBarIcon';
import {Colors} from '@/constants/Colors';
import {useColorScheme} from '@/hooks/useColorScheme';
import {Entypo} from "@expo/vector-icons";

export default function TabLayout() {
    const colorScheme = useColorScheme();

    return (
        <Tabs
            screenOptions={{
                tabBarActiveTintColor: Colors[colorScheme ?? 'light'].tint,
                headerShown: false,
            }}>
            <Tabs.Screen
                name="index"
                options={{
                    title: 'Home',
                    tabBarIcon: ({color, focused}) => (
                        <TabBarIcon name={focused ? 'home' : 'home-outline'} color={color}/>
                    ),
                }}
            />
            <Tabs.Screen
                name="explore"
                options={{
                    title: 'Explore',
                    tabBarIcon: ({color, focused}) => (
                        <TabBarIcon name={focused ? 'code-slash' : 'code-slash-outline'} color={color}/>
                    ),
                }}
            />
            <Tabs.Screen
                name="categories"
                options={{
                    title: 'Категорії',
                    tabBarIcon: ({color, focused}) => (
                        <TabBarIcon name={focused ? 'list' : 'list-outline'} color={color}/>
                    ),
                }}
            />
            <Tabs.Screen
                name="login"
                options={{
                    title: 'Вхід',
                    tabBarIcon: ({color, focused}) => (
                        <Entypo name="login" size={24} color={color}/>
                    ),
                }}
            />
            <Tabs.Screen
                name="registration"
                options={{
                    title: 'Реєстрація',
                    tabBarIcon: ({color, focused}) => (
                        <Entypo name="add-user" size={24} color={color}/>
                    ),
                }}
            />
        </Tabs>
    );
}
