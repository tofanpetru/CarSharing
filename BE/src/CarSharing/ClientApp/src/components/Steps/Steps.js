import React, { Component } from "react";
import './Steps.scss';

import Car1 from '../Images/Car1.png';
import Booking1 from '../Images/Booking1.png';
import Drive1 from '../Images/Drive1.png';
import Curved from '../Images/Curved.png';


export default class Steps extends Component {
    render() {

        return (
            <section className="steps-container">
                <div className="text-container">
                    <p className="rent">How to rent?</p>
                    <p className="rent-title">Make 3 Simple Steps to Rent a Car</p>
                    <div className="blue-line">
                        <div> </div>
                    </div>
                    <p className="rent-text">Especially to benefit from the trip with family or friends, </p>
                    <p className="rent-text">we reduced the number of steps for booking.</p>
                </div>


                    <div className="steps">
                        <div className="step1">
                            <img src={Car1} alt="car1" />
                            <p className="title">Choose a car</p>
                            <p className="description">Select the vehicle using our catalogues.</p>
                    </div>

                    <div className="step2">
                        <img className="curve" src={Curved} alt="curved" />
                    </div>

                    <div className="step3">
                        <img src={Booking1} alt="booking1" />
                        <p className="title">Make a booking</p>
                        <p className="description">Enter your name and booking details.</p>
                    </div>

                    <div className="step2">
                        <img className="curve" src={Curved} alt="curved" />
                    </div>

                    <div className="step5">
                        <img src={Drive1} alt="drive1" />
                        <p className="title">Enjoy your ride</p>
                        <p className="description">Enjoy your trip and our good servise!</p>
                    </div>


                    </div>


            </section>
        );
    }
}