#!/usr/bin/env bash

setup_git() {
    git config --global user.email "no-reply@github.com"
    git config --global user.name "Github-Actions[bot]"
}

update_nightly_branch() {
    printf "Run nightly update \n"
    git remote add origin-nightly "https://${GH_TOKEN}@github.com/ashblue/unity-elastic-inventory.git"
    git subtree split --prefix Assets/com.fluid.elastic-inventory -b nightly
    git push -f origin-nightly nightly:nightly
}

setup_git
update_nightly_branch
