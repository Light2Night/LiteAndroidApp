import {Button, StyleSheet, Text, TextInput, View} from 'react-native';
import React, {useState} from "react";
import axios from "axios";
import {getActionUrl} from "@/app/apiHelper";
import IJwtTokenResponse from "@/app/interfaces/IJwtTokenResponse";

export default function LoginScreen() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');

    const handleSignIn = () => {
        const form = new FormData();

        form.append('email', email);
        form.append('password', password);

        axios.postForm<IJwtTokenResponse>(getActionUrl('accounts', 'signin'), form)
            .then(response => {
                setError('');

                console.log(response.data.token);
            })
            .catch(error => {
                if (error.response.status === 401) {
                    setError('Invalid email or password');
                } else {
                    setError('Error');
                }
            });
    }

    return (
        <>
            <View style={styles.container}>
                <Text style={styles.header}>Вхід</Text>
                {error ? <Text style={styles.error}>{error}</Text> : null}

                <Text style={styles.title}>Логін</Text>
                <TextInput
                    value={email}
                    onChangeText={setEmail}
                    autoCapitalize="none"
                    keyboardType="email-address"
                    style={styles.input}
                />

                <Text style={styles.title}>Пароль</Text>
                <TextInput
                    value={password}
                    onChangeText={setPassword}
                    secureTextEntry
                    style={styles.input}
                />

                <Button
                    title="Sign In"
                    onPress={handleSignIn}
                />
            </View>
        </>
    );
}

const styles = StyleSheet.create({
    header: {
        backgroundColor: '#fff',
        textAlign: 'center',
        paddingTop: 40,
        fontSize: 24,
    },
    container: {
        flex: 1,
        justifyContent: 'center',
        padding: 16,
        backgroundColor: '#fff',
    },
    input: {
        marginBottom: 16,
        borderWidth: 1,
        borderColor: '#ccc',
        borderRadius: 4,
        padding: 8,
    },
    error: {
        color: 'red',
        marginBottom: 16,
        textAlign: 'center',
    },
    title: {
        fontSize: 16,
        paddingStart: 8
    }
});